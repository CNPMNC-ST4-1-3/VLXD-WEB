// Click table price 
var ximang = document.querySelector('.ximang');
var gach = document.querySelector('.gach');
var cat = document.querySelector('.cat');
var da = document.querySelector('.da');
var box = document.querySelector('.table-responsive');
var box1 = document.querySelector('.box');

var gachbutton = document.getElementById('gach-button');
var dabutton = document.getElementById('da-button');
var catbutton = document.getElementById('cat-button');
var ximangbutton = document.getElementById('ximang-button');

gachbutton.onclick = function () {
    ximang.style.visibility = 'hidden';
    cat.style.visibility = 'hidden';
    da.style.visibility = 'hidden';
    gach.style.visibility = 'visible';
    box.style.height = '250px';
}

dabutton.onclick = function () {
    ximang.style.visibility = 'hidden';
    cat.style.visibility = 'hidden';
    gach.style.visibility = 'hidden';
    da.style.visibility = 'visible';
    box.style.height = '450px';
}

ximangbutton.onclick = function () {
    ximang.style.visibility = 'visible';
    cat.style.visibility = 'hidden';
    gach.style.visibility = 'hidden';
    da.style.visibility = 'hidden';
    box.style.height = '450px';
    box1.style.height = '350px';
}
catbutton.onclick = function () {
    ximang.style.visibility = 'hidden';
    cat.style.visibility = 'visible';
    gach.style.visibility = 'hidden';
    da.style.visibility = 'hidden';
    box.style.height = '250px';
}