using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerInvoiceDetails;
using Indo.CustomerInvoices;
using Indo.NumberSequences;
using Indo.ProjectOrderDetails;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ProjectOrders
{
    public class ProjectOrderManager : DomainService
    {
        private readonly IProjectOrderRepository _projectOrderRepository;
        private readonly IProjectOrderDetailRepository _projectOrderDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        private readonly CustomerInvoiceManager _customerInvoiceManager;
        private readonly ICustomerInvoiceDetailRepository _customerInvoiceDetailRepository;
        private readonly CustomerInvoiceDetailManager _customerInvoiceDetailManager;
        private readonly IUomRepository _uomRepository;

        public ProjectOrderManager(
            IProjectOrderRepository projectOrderRepository,
            IProjectOrderDetailRepository projectOrderDetailRepository,
            ICustomerInvoiceRepository customerInvoiceRepository,
            CustomerInvoiceManager customerInvoiceManager,
            ICustomerInvoiceDetailRepository customerInvoiceDetailRepository,
            CustomerInvoiceDetailManager customerInvoiceDetailManager,
            IUomRepository uomRepository, 
            NumberSequenceManager numberSequenceManager
            )
        {
            _projectOrderRepository = projectOrderRepository;
            _projectOrderDetailRepository = projectOrderDetailRepository;
            _customerInvoiceRepository = customerInvoiceRepository;
            _customerInvoiceManager = customerInvoiceManager;
            _customerInvoiceDetailRepository = customerInvoiceDetailRepository;
            _customerInvoiceDetailManager = customerInvoiceDetailManager;
            _uomRepository = uomRepository;
            _numberSequenceManager = numberSequenceManager;
        }
        public async Task<ProjectOrder> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] Guid salesExecutiveId,
            [NotNull] DateTime orderDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(customerId, nameof(customerId));
            Check.NotNull<Guid>(salesExecutiveId, nameof(salesExecutiveId));
            Check.NotNull<DateTime>(orderDate, nameof(orderDate));

            var existing = await _projectOrderRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new ProjectOrderAlreadyExistsException(number);
            }

            return new ProjectOrder(
                GuidGenerator.Create(),
                number,
                customerId,
                salesExecutiveId,
                orderDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] ProjectOrder projectOrder,
           [NotNull] string newName)
        {
            Check.NotNull(projectOrder, nameof(projectOrder));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _projectOrderRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != projectOrder.Id)
            {
                throw new ProjectOrderAlreadyExistsException(newName);
            }

            projectOrder.ChangeName(newName);
        }

        public async Task<ProjectOrder> ConfirmAsync(Guid projectOrderId)
        {
            var obj = await _projectOrderRepository.GetAsync(projectOrderId);
            if (obj.Status == ProjectOrderStatus.Draft)
            {
                obj.Status = ProjectOrderStatus.Confirm;
                return await _projectOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<ProjectOrder> CancelAsync(Guid projectOrderId)
        {
            var obj = await _projectOrderRepository.GetAsync(projectOrderId);
            if (obj.Status == ProjectOrderStatus.Confirm)
            {
                obj.Status = ProjectOrderStatus.Cancelled;
                return await _projectOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }


        public async Task<CustomerInvoice> GenerateInvoice(Guid projectOrderId)
        {
            try
            {
                var order = await _projectOrderRepository.GetAsync(projectOrderId);
                if (order.Status == ProjectOrderStatus.Confirm)
                {

                    var invoice = await _customerInvoiceManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Invoice),
                            order.CustomerId,
                            DateTime.Now,
                            DateTime.Now.AddDays(14)
                        );

                    invoice.SourceDocument = order.Number;
                    invoice.SourceDocumentId = order.Id;
                    invoice.SourceDocumentModule = NumberSequenceModules.ProjectOrder;

                    var result = await _customerInvoiceRepository.InsertAsync(invoice, true);

                    var queryable = await _projectOrderDetailRepository.GetQueryableAsync();
                    var query = from projectOrderDetail in queryable
                                where projectOrderDetail.ProjectOrderId == order.Id
                                select new { projectOrderDetail };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var details = new List<CustomerInvoiceDetail>();
                    var uom = _uomRepository.FirstOrDefault();
                    foreach (var item in queryResult)
                    {
                        var detail = await _customerInvoiceDetailManager.CreateAsync(
                                result.Id,
                                item.projectOrderDetail.ProjectTask,
                                uom.Id,
                                item.projectOrderDetail.Price,
                                0,
                                item.projectOrderDetail.Quantity,
                                0
                            );
                        details.Add(detail);

                    }
                    await _customerInvoiceDetailRepository.InsertManyAsync(details, true);

                    return invoice;
                }
                else
                {
                    throw new UserFriendlyException("Only Confirm can be processed!");
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Error: " + ex.Message);
            }
        }


    }
}
