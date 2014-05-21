$(document).ready(function () {


    $('#btnCancel').click(function () {
        
        $('#ddTimeoffType')[0].selectedIndex = 0;
        $("#rbFullDay").prop("checked", true);        
        $('#txtStartDate').val('');
        $('#txtEndDate').val('');
        $("#rbExcludeWeekendYes").prop("checked", true);
        $("#rbExcludeHolidayYes").prop("checked", true);
        $("#rbExcludeOtherYes").prop("checked", true);   
      
        if ($('#ddlsrttime').length != 0)
            $('#ddlsrttime').hide();
        if ($('#ddlendtime').length != 0)
            $('#ddlendtime').hide();
        
        $('#ddApprover1')[0].selectedIndex = 0;
        $('#ddApprover2')[0].selectedIndex = 0;
        $('#ddApprover3')[0].selectedIndex = 0;

       
    });

    function checkDisabled(evt) {
        var val = $("input[name=FullOrPartial]:checked").val();
        if (val == 'rbFullDay') {
            if ($('#ddlsrttime').length != 0)
                $('#ddlsrttime').hide();
            if ($('#ddlendtime').length != 0)
                $('#ddlendtime').hide();
        } else {
            $('#ddlsrttime').show();
            $('#ddlendtime').show();
            var d = new Date();
            var date = (d.getMonth() + 1) + '/' + d.getDate() + '/' + d.getFullYear();
            $('#txtStartDate').val(date);
            $('#txtEndDate').val(date);
        }
    }
  
    $('input[name=FullOrPartial]:radio').change(checkDisabled);
         checkDisabled();
    });


$('#back').click(function () {
    history.back();
    return false;
});




