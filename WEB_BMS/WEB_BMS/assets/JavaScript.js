// click

var danhgia = document.querySelector('.danhgia');
var luotdanhgia = document.querySelector('.luotdanhgia');

var luotdanhgiabutton = document.getElementById('button-luotdanhgia');
var danhgiabutton = document.getElementById('button-danhgia');



luotdanhgiabutton.onclick = function () {
    danhgia.style.visibility = 'hidden';
    luotdanhgia.style.visibility = 'visible';
}


danhgiabutton.onclick = function () {
    danhgia.style.visibility = 'visible';
    luotdanhgia.style.visibility = 'hidden';
}


