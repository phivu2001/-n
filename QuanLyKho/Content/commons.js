function ConfirmSwalMessage(title, message, btnConfirmText, btnCancelText, action) {
    Swal.fire({
        title: title,
        text: message,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: btnConfirmText,
        cancelButtonText: btnCancelText,
    }).then((result) => {
        if (result.value) {
            window.location.href = action;
        }
    })
}

function ToastMessage(message, type) {
    $.toast({
        text: message,
        icon: type, // 'error','warning','success','info'
        showHideTransition: 'fade',
        allowToastClose: true,
        hideAfter: 5000,
        stack: 10,
        position: 'top-right',
        textAlign: 'left',
        loader: true,
        loaderBg: '#9EC600',
    });
}

$(function () {
    dongnguyenxanh.countOrder();
});

$('.show-cart').on('click', function (e) {
    e.preventDefault();
	var Items = localStorage.getItem("cart");	
	var html = "";
	var total = 0;
	if (Items != null) {
		var Orders = JSON.parse(Items);
        $.each(Orders, function (index, _item) {
            html += "<div class='single-item' id='" + _item.Id + "'>"
				+ "<div class='image'>" + "<a href=''>" + "<img style='width: 80px; height: 80px; ' src='" + _item.ImageURL.substring(1, _item.ImageURL.length) + "' />" + "</a>"
				+ "</div>"
				+ "<div class='content'>"
				+ "<p class='cart-name'><a href=''>" + _item.Name + "</a></p>"
                + "<p class='cart-quantity'><span class='quantity-mes'>" + _item.Qty + " x </span>" + NumberFormat(parseFloat(_item.Price)) + "</p>"
                + "</div>"
                + "<a href='javascript:void(0)' onclick='RemoveItemCart(\"" + _item.Id + "\")' class='remove-icon'><i class='ion-close-round'></i></a>"
				+ "</div>";
			total = parseFloat(total) + (parseFloat(_item.Price) * _item.Qty);
        });
		$('.small-cart-item-wrapper').html(html);
        $('.clss-subtotal').text(NumberFormat(total));
        $('.clss-total').text(NumberFormat(total));
		localStorage.removeItem("cart");
		var Json = JSON.stringify(Orders);
		localStorage.setItem("cart", Json);
	}
})

var dongnguyenxanh = {
    addToCard: function (item) {
        var Items = localStorage.getItem("cart");
        if (Items != null) {
            var Orders = JSON.parse(Items)
            var hasItem = Orders.some((o) => { return (o.Id == item.Id) })
            if (hasItem == false) {
                Orders.push(item)
            }
            else {
                $.each(Orders, function (index, _item) {
                    if (_item.Id === item.Id) {
                        _item.Qty = parseInt(_item.Qty) + parseInt(item.Qty)
                    }
                });
            }
            localStorage.removeItem("cart")

            var Json = JSON.stringify(Orders)
            localStorage.setItem("cart", Json);
        } else {
            /*** Created new localStorage ***/
            var _Items = []
            _Items.push(item)
            var Json = JSON.stringify(_Items)
            localStorage.setItem("cart", Json);
        }
        /*** UPDATE NUMBER SHOPPING CART ***/
        this.countOrder()
    },
    getListOrder: function () {
        var Items = localStorage.getItem("cart");
        var Orders = null
        if (Items != null) {
            Orders = JSON.parse(Items)
        }
        return Orders
    },
    countOrder: function () {
        var Items = localStorage.getItem("cart");
        var Total = 0
        if (Items != null) {
            var Orders = JSON.parse(Items)
            $.each(Orders, function (index, _item) {
                Total = Total + parseInt(_item.Qty)
            });

        }
        $('.cart-counter').text(Total)
    },
    updateQtyOrder: function (id, quantity) {
        var Items = localStorage.getItem("cart");
        var Total = 0;
        if (Items != null) {
            var Orders = JSON.parse(Items);
            $.each(Orders, function (index, _item) {
                if (_item.Id === id) {
                    _item.Qty = quantity;
                    Total = Total + quantity;
                } else {
                    Total = Total + parseInt(_item.Qty);
                }

            });
            localStorage.removeItem("cart");
            var Json = JSON.stringify(Orders);
            localStorage.setItem("cart", Json);
        }
        $('.count').text(Total);
    },
    deleteOrder: function (id) {
        var Items = localStorage.getItem("cart");
        var Total = 0;
        if (Items != null) {
            var Orders = JSON.parse(Items);
            var indexItem = -1;
            $.each(Orders, function (index, _item) {
                if (_item.Id === id) {
                    indexItem = index;
                } else {
                    Total = Total + parseInt(_item.Qty);
                }
            });
            if (indexItem !== -1) {
                Orders.splice(indexItem, 1);
            }
            localStorage.removeItem("cart");
            var Json = JSON.stringify(Orders);
            localStorage.setItem("cart", Json);
        }
        $('.count').text(Total);
    },
    detroyOder: function () {
        localStorage.removeItem("cart");
        $('.count').text(0);
    }
}


function RemoveItemCart(id) {
    debugger;
    var Total = 0;
    var totalPrice = 0;
    $('#' + id).css("display", "none");
    dongnguyenxanh.deleteOrder(id);    
    var Orders = dongnguyenxanh.getListOrder();
    $.each(Orders, function (index, _item) {
        Total += parseFloat(Total) + parseInt(_item.Qty);
        totalPrice += parseFloat(totalPrice) + (parseFloat(_item.Price) * _item.Qty);
    });
    $('.cart-counter').text(Total);
    $(".clss-subtotal").text(NumberFormat(totalPrice))
    $(".clss-total").text(NumberFormat(totalPrice))
    //UpdateTotalCount(dataProduct);
}

function NumberFormat(number) {
    return new Intl.NumberFormat().format(number);
}

$('.quickView').on('click', function () {
    var handle = $(this).attr("data-handle")
    $.ajax({
        url: handle,
        success: function (data) {
            //if (data != null) {
            //    var htmlBigImg = "";
            //    var htmlSmallImg = "";
            //    var Orders = JSON.parse(data.data);
            //    if (Orders.ListImages != null && Orders.ListImages.length > 0) {
            //        for (var i = 0; i < Orders.ListImages.length; i++) {
            //            htmlBigImg += "<div class='big-image-slider-single-item'>" + "<img src='" + Orders.ListImages[i].ImageURL.substring(1, Orders.ListImages[i].ImageURL.length) + "' class='img-fluid' alt=''>" + "</div>";
            //            htmlSmallImg += "<div class='small-image-slider-single-item'>" + "<img src='" + Orders.ListImages[i].ImageURL.substring(1, Orders.ListImages[i].ImageURL.length) + "' class='img-fluid' alt=''>" + "</div>";
            //        }
            //    }
            //    $('.big-image-slider').html(htmlBigImg);
            //    $('.small-image-slider').html(htmlBigImg);
            //}
            $("#modal-body-content").html(data);
            $('#quick-view-modal-container').modal('show');
        }
    });
})
