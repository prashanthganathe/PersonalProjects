//(function () {
//    var resourceModel = function () {
//        this.id;
//        this.unitModel;     
//        this.seatType;
//        this.legend;
//        this.quantity;
//    };
//}());

var resourceModel = resourceModel || {};
resourceModel = function(){
        id='';
        unitModel='';
        seatType='';
        legend='';
        quantity='';
}

function getResouceArray()
{
    var rescArray = [];
    var tempResourceModel;
    for (i = 0; i < 6;i++) {
        tempResourceModel = new resourceModel();
        tempResourceModel.id=i;
        tempResourceModel.unitModel = 'unitModel' + i;
        tempResourceModel.seatType = 'Cabin' + i;
        tempResourceModel.legend = 'legend' + i;
        tempResourceModel.quantity = i;

        rescArray.push(tempResourceModel);
    }
    return rescArray;
    
}