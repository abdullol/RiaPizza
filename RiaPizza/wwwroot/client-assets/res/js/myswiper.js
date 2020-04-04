
var lastCat = "cat1";
var mySwiper = new Swiper(".swiper-container", {
    //slidesPerView: 'auto',
    slidesPerView: 'auto',
    spaceBetween: 20,
    freeMode: false,
    loop: false,
    watchOverflow: !0,
    navigation: {
        nextEl: ".swiper-next",
        prevEl: ".swiper-prev"
    }
});

//mySwiper.slidesPerView = 8;
mySwiper.update();

mySwiper.on("reachBeginning", function () {
    console.log("Swiper Beginning reached");
});

mySwiper.on("reachEnd", function () {
    console.log("Swiper end reached");
});

function next() {
    // mySwiper.slideTo(count++);
    console.log("in next");

    mySwiper.slideNext();
}

// ! Showing/Hiding Search Bar
function showSearchBar() {
    var searchBtn = document.getElementById("search-btn");
    var dishList = document.getElementById("dish-list");
    var searchBox = document.getElementById("search-box");
    $('.swiper-next').hide();
    $('#mainNav').css('height', '75px');

    searchBtn.style.display = "none";
    dishList.style.display = "none";
    searchBox.style.display = "flex";
}

function hideSearchBar() {
    var searchBtn = document.getElementById("search-btn");
    var dishList = document.getElementById("dish-list");
    var searchBox = document.getElementById("search-box");
    $('.swiper-next').show();
    $('#mainNav').css('height', '60px');

    var input = document.getElementById('search-dish');
    input.value = "";
    spliceSearch(input);

    searchBtn.style.display = "flex";
    dishList.style.display = "flex";
    searchBox.style.display = "none";
}

function spliceSearch(el) {
    var value = $(el).val().toLowerCase();
    $(".dish-head").hide();
    $(".dish-head").each(function () {
        let val = $(this).data('categoryhead');
        $('.dishes[data-category="' + val + '"]').each(function () {
            var head = $(this).data('category');

            if ($(this).text().toLowerCase().indexOf(value) > -1) {
                $(this).show();
                $('.' + head).show();
            }
            else
                $(this).hide();
        });
    });
}

$(document).ready(function () {
    $('.dishlink').css({ "width": "auto" })
    $("#search-dish").on("keyup", function () {
        spliceSearch(this);
    });
    var waypoints = $(".group-item ").waypoint({
        handler: function (direction) {
            var index = this.element.getAttribute("index");

            if (direction === "down") {
                index = parseInt(index) - 1;
            } else {
                index = parseInt(index) - 2;
            }

            mySwiper.slideTo(index);
            console.log("Last Cat2 : " + index);
        }
    });
});

$(window).onscroll = function () {
    $('.dishlink').css({ "width": "auto" });
    scrollFunction();
};