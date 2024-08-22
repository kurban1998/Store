$(document).ready(function () {
    $('.remove-item').click(function (e) {
        e.preventDefault();

        var itemId = $(this).data('item-id');

        $.ajax({
            url: '/Basket/DeleteItemFromBasket',
            type: 'POST',
            data: { itemId: itemId },
            success: function (response) {
                var row = $('button[data-item-id="' + itemId + '"]').closest('tr');
                row.remove();

                if ($('table tbody tr').length === 0) {
                    $('#no-items-in-basket').show();
                    $('#create-order-button').hide();
                } else {
                    $('#create-order-button').show();
                }
            },
            error: function (xhr, status, error) {
                alert('Ошибка при удалении товара: ' + error);
            }
        });
    });
});