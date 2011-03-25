function initialize() {
    var myLatlng = new google.maps.LatLng(36.288563, 138.427734,5);
    var myOptions = {
        zoom: 4,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    var api = new readingPointsApi();
    function Scale(reading)
    {
        if ( 0 < reading < 50)
            return 'green';
        if (50 < reading < 100)
            return 'orange';
        if (100 < reading < 250)
            return 'red';
        return 'delete';
    }
    var readings = api.getAllReadings(
    { 
        success: function (result) 
        {

            for (var i = 0; i < result.length; i++) 
            {
                var image = '/Content/images/bullet_' + Scale(result[i].Reading) + '.png';
                var message = 'lat: ' + result[i].Latitude + ', ' + 'long: ' + result[i].Longitude;
                var readingPoint = new google.maps.LatLng(result[i].Latitude, result[i].Longitude);
                var beachMarker = new google.maps.Marker({
                    position: readingPoint,
                    map: map,
                    icon: image
                });
                var contentString = '<div>' + result[i].Reading + ' mSv</div>';

                var infowindow = new google.maps.InfoWindow({
                    content: contentString
                });

                google.maps.event.addListener(beachMarker, 'click', function () 
                {
                    infowindow.open(map, beachMarker);
                });
            }
        }
    });

}