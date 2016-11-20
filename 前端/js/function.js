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

    $(".notice_head .delete_box").click(function(){
        $(".move_box").animate({left:'20px'});
        $(".checkbox").animate({left:'15px'});
        $(".none").show();

    });
    $(".back").click(function(){
        $(".move_box").animate({left:'0'});
        $(".checkbox").animate({left:'-20px'});
        $(".none").hide();

    });



});

