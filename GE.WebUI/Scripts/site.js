function changeLocation(element) {
    var href = $(element).find('a').attr('href');
    window.location = href;
}