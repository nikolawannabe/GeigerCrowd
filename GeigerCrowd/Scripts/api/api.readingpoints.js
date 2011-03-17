var readingPointsApi = function () { }

readingPointsApi.prototype.getAllReadings = function (options) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: '/api/getAllReadings',
        success: function (result) { config.success(result); }
    });
}