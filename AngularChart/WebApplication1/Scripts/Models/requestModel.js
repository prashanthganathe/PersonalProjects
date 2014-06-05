//(function () {
//    var requestModel = function () {
//        this.id;
          
//        this.process;
//        this.from;
//        this.to;
//        this.projectCode;
//        this.resourceModel = [];
//        this.employeeid;
//    };
//}());

var requestModel = requestModel || {};
requestModel = function(){
    id='';
    process='';
    from='';
    to='';
    projectCode='';
    resourceModel=[];
    employeeid = '';
}

function getRequestArray() {
    var requestModelArray=[];
   // var tempRequestModel = new requestModel();
    var d = new Date();
    for (var i = 0; i < 14;i++) {
        temprequest= new requestModel();
        temprequest.id=i;
        temprequest.process='process'+i;
        temprequest.from=   d.setDate(d.getDate() - 1 -i);
        temprequest.to=   d.setDate(d.getDate() +i);
        temprequest.temprequest = 'projectCode_'+i;
       // temprequest.resourceModel = getResouceArray();
        requestModelArray.push(temprequest);
    }
    return requestModelArray;
}
