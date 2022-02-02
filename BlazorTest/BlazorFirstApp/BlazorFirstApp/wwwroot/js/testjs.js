$(function () {
    $('#setValues').click(function () {
        $('li').each(function () {
            var $elem = $(this);
            $elem.attr('originalValue', $elem.text());
        });
    });
});
