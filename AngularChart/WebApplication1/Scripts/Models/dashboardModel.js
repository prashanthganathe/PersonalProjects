'use strict';

(function () {
    var dashboardTypeModel = function () {
        this.seatUtilization = 0;
        this.seatUnUtilization = 0;
        this.seatswithLessthan50 = 0;
        this.costTrend;
        this.seatSwapRequests;
        this.vacantSeatTrend;
        this.perFTECostTrend;
        this.seatsSplit;
        this.requestlist = [];
    };
}());