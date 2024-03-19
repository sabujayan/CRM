$(".g-modal").hide();
$(".g-modal-nav").hide();
$(".lg-block .lg-modal-block").hide();

let avoid="#";

$(".det-btn").click(function(){
    $(".g-modal").show();
    
    let content = $(this).parents(".lg-block").find(".lg-modal-block").html();
    let link = $(this).attr('href');
    link_ed = link.replace(avoid,'');
    $(".g-modal-content").append("<div class='g-img-l' id='"+link_ed+"'>"+content+"</div>");

    if($(this).parent().children().length > 1){
        $(".g-modal-nav").show();
    }else{
        $(".g-modal-nav").hide();
    }
    
});

$(".mod-next").click(function(){
    let id = $(this).parents(".g-modal").find(".g-img-l").attr('id');

    let total = $('a[href*="'+id+'"]').parents(".light-gallery").children().length;
    // console.log(total);
    let current = $('a[href*="'+id+'"]').parents(".lg-block").index() + 1;
    // alert(current);

    if(total == current){ // when reach last element
        let fcontent = $('a[href*="#'+id+'"]').parents(".light-gallery").children(':first-child').find(".lg-modal-block").html();
        let link = $('a[href*="'+id+'"]').parents(".light-gallery").children(':first-child').find(".det-btn").attr('href');
        let link_ed = link.replace(avoid,'');
        $(".g-modal-content .g-img-l").replaceWith( $( "<div class='g-img-l' id='"+link_ed+"'>"+fcontent+"</div>" ) );

    }else{
        let content = $('a[href*="#'+id+'"]').parents(".lg-block").next().find(".lg-modal-block").html();
        let link = $('a[href*="'+id+'"]').parents(".lg-block").next().find(".det-btn").attr('href');
        let link_ed = link.replace(avoid,'');
        $(".g-modal-content .g-img-l").replaceWith( $( "<div class='g-img-l' id='"+link_ed+"'>"+content+"</div>" ) );
    }

});

$(".mod-prev").click(function(){
    let id = $(this).parents(".g-modal").find(".g-img-l").attr('id');

    let current = $('a[href*="'+id+'"]').parents(".lg-block").index();
    // alert(current);

    if(current < 1){ // when reach first element
        let fcontent = $('a[href*="#'+id+'"]').parents(".light-gallery").children(':last-child').find(".lg-modal-block").html();
        // alert(fcontent);
        let link = $('a[href*="'+id+'"]').parents(".light-gallery").children(':last-child').find(".det-btn").attr('href');
        let link_ed = link.replace(avoid,'');
        $(".g-modal-content .g-img-l").replaceWith( $( "<div class='g-img-l' id='"+link_ed+"'>"+fcontent+"</div>" ) );

    }else{
        let content = $('a[href*="#'+id+'"]').parents(".lg-block").prev().find(".lg-modal-block").html();
        let link = $('a[href*="'+id+'"]').parents(".lg-block").prev().find(".det-btn").attr('href');
        let link_ed = link.replace(avoid,'');
        $(".g-modal-content .g-img-l").replaceWith( $( "<div class='g-img-l' id='"+link_ed+"'>"+content+"</div>" ) );
    }
});

$(".g-close-modal").click(function(){
    $(".g-modal").hide();
    $(".g-modal-content").empty();
    $(".g-modal-nav").hide();
});


