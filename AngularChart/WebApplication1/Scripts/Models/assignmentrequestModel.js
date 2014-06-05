'use strict';

var assignmentrequestModel = assignmentrequestModel || {};

var assignmentrequestModel = function (id, ResourceId, UserId,UserName, Process, ProjectCode, Location, Office, From, To, Legend, Remarks, AssignedOn, AssignedBy, SeatStatus, Currency, TaxType) {

    this.Id = id;
    this.ResourceId = ResourceId;
    this.UserId = UserId;
    this.UserName = UserName;
    this.Process = Process;
    this.ProjectCode = ProjectCode;
    this.Location = Location;
    this.Office = Office;
    this.From = From;
    this.To = To;
    this.Legend = Legend;
    this.Remarks = Remarks;
    this.AssignedOn = AssignedOn;
    this.AssignedBy = AssignedBy;
    this.SeatStatus = SeatStatus;
    this.Currency = Currency;
    this.TaxType = TaxType;
}

assignmentrequestModel.prototype.getDummyData = function () {
  
   return [
                     new assignmentrequestModel('1', 'R2', 'U1', 'proc1', 'Proj1', 'loc1', 'off1', '06/04/2014', '2014/04/04', 'Split Seat', '', '2014-02-02', 'ABC', 'Reserved', '$USD', 'VAT'),
                     new assignmentrequestModel('2', 'R3', 'U1', 'proc1', 'Proj1', 'loc1', 'off2', '06/27/2014', '2014/06/04', 'Hot Seat', '', '2014-02-02', 'ABC', 'Reserved', '$USD', 'VAT'),
                     new assignmentrequestModel('3', 'R121', 'U1', 'proc2', 'Proj1', 'loc2', 'off3', '06/06/2014', '06/09/2014', 'Cold Seat', '', '2014-02-02', 'ABC', 'Reserved', '$USD', 'VAT'),
                     new assignmentrequestModel('4', 'R143', 'U1', 'proc3', 'Proj1', 'loc2', 'off4', '2014/02/20', '2014/04/04', 'Blocked Seat', '', '2014-02-02', 'ABC', 'Reserved', '$USD', 'VAT'),
                     new assignmentrequestModel('5', 'R132', 'U1', 'proc3', 'Proj1', 'loc3', 'off5', '2014/02/20', '2014/04/04', 'Split Seat', '', '2014-02-02', 'ABC', 'Reserved', '$USD', 'VAT'),

          ];


};

