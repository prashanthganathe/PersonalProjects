$(document).ready(function () {

    $(document).on("click", 'li', function () {
        $(".menu>li.active").removeClass("active");
        $(this).addClass("active");
    });
});