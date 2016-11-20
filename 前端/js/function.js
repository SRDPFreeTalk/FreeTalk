/**
 * Created by 17922 on 16/11/19.
 */
$(document).ready(function(){
    $(".showMenu").click(function(){
        $(".menu").slideToggle();
        $(".logo_onClick").show();
    });
    $(document).click(function(){
        $(".menu").slideUp();
        $(".logo_onClick").hide();
    });
    $(".menu,.showMenu").click(function(event){
        event.stopPropagation();

    });
    $(".thumbs_up").click(function(){
        $(this).toggleClass('thumbs_bg1').toggleClass('thumbs_bg2');
    });
    $(".chat").click(function(){
        $(this).toggleClass('chat_bg1').toggleClass('chat_bg2');

    });
    $(".input_main textarea").bind("keydown keyup",function(){
        $(this).autosize();
    }).show().autosize();

    $.fn.autosize = function(){
        $(this).height('0px');
        var setheight = $(this).get(0).scrollHeight;
        if($(this).attr("_height") != setheight)
            $(this).height(setheight+"px").attr("_height",setheight);
        else
            $(this).height($(this).attr("_height")+"px");
    }


});

