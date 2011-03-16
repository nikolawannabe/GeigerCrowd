function initialize() {
    var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
    var myOptions = {
        zoom: 4,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

    var image = '/Content/images/bullet_green.png';
    var myLatLng = new google.maps.LatLng(-33.890542, 151.274856);
    var beachMarker = new google.maps.Marker({
        position: myLatLng,
        map: map,
        icon: image
    });

    var contentString = '4.2';

    var infowindow = new google.maps.InfoWindow({
        content: contentString
    });


google.maps.event.addListener(beachMarker, 'click', function () {
    infowindow.open(map, beachMarker);
});
}