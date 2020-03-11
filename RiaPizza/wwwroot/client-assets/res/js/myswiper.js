var lastCat = "cat1";
var mySwiper = new Swiper(".swiper-container", {
  // slidesPerView: 'auto',
  slidesPerView: 6,
  spaceBetween: 30,
  freeMode: !0,
  loop: !1,
  watchOverflow: !0,
  navigation: {
    nextEl: ".swiper-next",
    prevEl: ".swiper-prev"
  }
});

mySwiper.slidesPerView = 8;
mySwiper.update();

mySwiper.on("reachBeginning", function() {
  console.log("Swiper Beginning reached");
});

mySwiper.on("reachEnd", function() {
  console.log("Swiper end reached");
});

function next() {
  // mySwiper.slideTo(count++);
  console.log("in next");

  mySwiper.slideNext();
}

var waypoints = $(".group-item ").waypoint({
  handler: function(direction) {
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

function myFunction() {
  var dots = document.getElementById("dots");
  var moreText = document.getElementById("more");
  var btnText = document.getElementById("myBtn");

  if (dots.style.display === "none") {
    dots.style.display = "inline";
    btnText.innerHTML = "Read more";
    moreText.style.display = "none";
  } else {
    dots.style.display = "none";
    btnText.innerHTML = "Read less";
    moreText.style.display = "inline";
  }
}

// ? Show/Less grid checboxes
function show() {
  var moreChecks = document.getElementById("more-checks");
  var btn = document.getElementById("more-btn");
  var btn2 = document.getElementById("less-btn")

  moreChecks.style.display = "block";
  btn.style.display = "none"
  btn2.style.display = "block"
}
function less() {
  var moreChecks = document.getElementById("more-checks");
  var btn = document.getElementById("more-btn");
  var btn2 = document.getElementById("less-btn")

  moreChecks.style.display = "none";
  btn.style.display = "block"
  btn2.style.display = "none"
}

// ! Showing/Hiding Search Bar
function showSearchBar() {
  var searchBtn = document.getElementById("search-btn");
  var dishList = document.getElementById("dish-list");
  var searchBox = document.getElementById("search-box") ;
  var angleLeft = document.getElementById("swiper-next")

  searchBtn.style.display = "none";
  dishList.style.display = "none";
  angleLeft.style.display = "none";
  searchBox.style.display = "flex";
}

function hideSearchBar() {
  var searchBtn = document.getElementById("search-btn");
  var dishList = document.getElementById("dish-list");
  var searchBox = document.getElementById("search-box") ;
  var angleLeft = document.getElementById("swiper-next")

  searchBtn.style.display = "flex";
  dishList.style.display = "flex";
  angleLeft.style.display = "flex";
  searchBox.style.display = "none";
}
