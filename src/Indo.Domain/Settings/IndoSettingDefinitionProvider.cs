using Volo.Abp.Settings;

namespace Indo.Settings
{
    public class IndoSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(IndoSettings.MySetting1));
        }
    }
}
