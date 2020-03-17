// ! Hiding the first modal while the second modal is shown
$(".modal-child").on("show.bs.modal", function() {
  var modalParent = $(this).attr("data-modal-parent");
  $(modalParent).css("opacity", 0);
});

// ? Showing the first modal after the second modal is closed
$(".modal-child").on("hidden.bs.modal", function() {
  var modalParent = $(this).attr("data-modal-parent");
  $(modalParent).css("opacity", 1);
});

// * Get the button for scrolling to top
var mybutton = document.getElementById("myBtn");

// ? Get Sidebar
var sideBar = document.getElementById("sidebar");

// ? When the user scrolls down 20px from the top of the document, show the button and fix the position of sidebar on scroll
window.onscroll = function() {
  scrollFunction();
};

function scrollFunction() {
  if (document.body.scrollTop > 75 || document.documentElement.scrollTop > 75) {
    mybutton.style.display = "block";
    sideBar.style.top = "0";
  } else {
    mybutton.style.display = "none";
    sideBar.style.top = "";
  }
}

// ? When the user clicks on the button, scroll to the top of the document
function topFunction() {
  document.body.scrollTop = 0;
  document.documentElement.scrollTop = 0;
}

// ? Maintaining the size of the document still when the virtual keyboard on mobile devices is active
var isAndroid = navigator.userAgent.toLowerCase().indexOf("android") > -1; //&& ua.indexOf("mobile");
if (isAndroid) {
  document.write(
    '<meta name="viewport" content="width=device-width,height=' +
      window.innerHeight +
      ', initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">'
  );
}

// ! Changing the color of heart on click in product page
$("#heart").click(function() {
  $(".heart").toggleClass("fas heart-color");
});

// ? Adjusting the width of popups on all devices dynamically
$(document).ready(function() {
  var modalWidth = $(window).width();
  if (modalWidth < 900) {
    $(".my-modal").attr(
      "style",
      "width: " + modalWidth + "px !important; margin: 0 auto;"
    );
    $(".info-modal").attr(
      "style",
      "width: " + modalWidth + "px !important; margin: 0 auto;"
    );
  }
});

// ? Adjusting the width of popups on all mobile devices dynamically when the orientation is changed to landscape mode
$(window).on("orientationchange", function() {
  var modalWidth = $(window).width();
  if (window.innerHeight > window.innerWidth) {
    modalWidth = $(window).height();
  }

  if (modalWidth < 900) {
    $(".my-modal").attr(
      "style",
      "width: " + modalWidth + "px !important; margin: 0 auto;"
    );
    $(".info-modal").attr(
      "style",
      "width: " + modalWidth + "px !important; margin: 0 auto;"
    );
  }
});

// ? Adding the active class to the clicked tab in about restaurant popup
$(".tab-link").click(function() {
  $(".tab-link").removeClass("active-tab");
  $(this).addClass("active-tab");
});

// *  Adding and subtracting the order quantity
jQuery(document).ready(function() {
  // ? This button will increment the value
  $(".qtyplus").click(function(e) {
    // ! Stop acting like a button
    e.preventDefault();
    // ? Get the field name
    fieldName = $(this).attr("field");
    // ! Get its current value
    var currentVal = parseInt($("input[name=" + fieldName + "]").val());
    // ? If is not undefined
    if (!isNaN(currentVal)) {
      // ! Increment
      $("input[name=" + fieldName + "]").val(currentVal + 1);
    } else {
      // ? Otherwise put a 0 there
      $("input[name=" + fieldName + "]").val(0);
    }
  });
  // * This button will decrement the value till 0
  $(".qtyminus").click(function(e) {
    // ? Stop acting like a button
    e.preventDefault();
    // * Get the field name
    fieldName = $(this).attr("field");
    // ? Get its current value
    var currentVal = parseInt($("input[name=" + fieldName + "]").val());
    // ! If it isn't undefined or its greater than 0
    if (!isNaN(currentVal) && currentVal > 0) {
      // ? Decrement one
      $("input[name=" + fieldName + "]").val(currentVal - 1);
    } else {
      // ! Otherwise put a 0 there
      $("input[name=" + fieldName + "]").val(0);
    }
  });
});

$(function() {
  $('.acc_ctrl').on('click', function(e) {
    e.preventDefault();
    if ($(this).hasClass('active')) {
      $(this).removeClass('active');
      $(this).next()
      .stop()
      .slideUp(300);
    } else {
      $(this).addClass('active');
      $(this).next()
      .stop()
      .slideDown(300);
    }
  });
});