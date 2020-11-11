
$(document).ready(function() {

  /*$('input[name="daterange"]').daterangepicker({
    opens: 'left'
  }, function(start, end, label) {
    console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
  });*/

   if(typeof start == 'undefined') var start = moment().subtract(1,'month').startOf('month');
   if(typeof end == 'undefined') var end = moment().subtract(1,'month').endOf('month');

    function cb(start, end) {
        $('input[name="daterange"]').html(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));
    }

    $('input[name="daterange"]').daterangepicker({
        startDate: start,
        endDate: end,
        minDate: '01/01/2013',
        maxDate: moment(),
        dateLimit: { days: 3000 },
        showDropdowns: true,
        ranges: {
           'This Month': [moment().subtract(0,'month').startOf('month'), moment().subtract(1,'days')],
           'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
           'Last Quarter': [moment().subtract(1, 'Q').startOf('Q'), moment().subtract(1,'Q').endOf('Q')],
           'Last 6 Months': [moment().subtract(6, 'month').startOf('month'), moment().subtract(1,'month').endOf('month')],
           'This Year': [moment().startOf('year'), moment().subtract(1,'days')],
           'Last Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
           'Last 5 Years': [moment().subtract(5, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
        }
    }, cb);

    cb(start, end);

 

//});

//$(document).ready(function () {
  $('select[name="province"]').change(function (e) {
    
    for(i=0; i<$('#district option').length;i++){
    $('#district option')[i].style.display="none";
    }
     var selectedClass = $(this).val();
     document.getElementById('district').selectedIndex=0;
      for(i=0; i<$("."+selectedClass).length;i++)
       $("."+selectedClass)[i].style.display="block";
       
  });

  $('select[name="district"]').change(function (e) {
    
    if(document.getElementById('province').selectedIndex==0){
    alert("Province Not Specified");
    document.getElementById('district').selectedIndex=0;
    }
    else{
    for(j=0; j<$('#facility option').length;j++)
    $('#facility option')[j].style.display="none";

    
     var selectClass = $(this).val();
     document.getElementById('facility').selectedIndex=0;
     for(i=0; i<$("."+selectClass).length;i++)
       $("."+selectClass)[i].style.display="block";
    
    }
});

$('select[name="facility"]').click(function (e) {
    
  if(document.getElementById('province').selectedIndex==0 || document.getElementById('district').selectedIndex==0){
    alert("Either Province or District Not Specified");
  document.getElementById('facility').selectedIndex=0;
  }
  
});
  


function renderChart(){
  if(document.getElementById('agbarchart')){
  var ctx = document.getElementById('agbarchart').getContext('2d');
  var barchart = new Chart(ctx,
                          {
                            type: 'horizontalBar',
                            data: {
                              labels: labels,
                              datasets: [{
                                label: 'Male',
                                data: data_m,
                                backgroundColor: '#3e95cd'
                              },{
                                label: 'Female',
                                data: data_f,
                                backgroundColor: '#8e5ea2'
                              },{
                                label: 'Indeterminate',
                                data: data_i,
                                backgroundColor: '#3cba9f'
                              },{
                                label: 'Unknown',
                                data: data_u,
                                backgroundColor: '#e8c3b9'
                              },{
                                label: 'Missing',
                                data: data_x,
                                backgroundColor: '#c45850'
                              }]
                            },
                            options: {
                              tooltips: {

                              },
                              scales:{
                                xAxes: [
                                  {
                                    stacked: true,
                                    scaleLabel: {
                                      display: true,
                                      labelString: 'Number of Tests'
                                    }
                                  }
                                ],
                                yAxes: [
                                  {
                                    stacked: true,
                                    scaleLabel: {
                                      display: true,
                                      labelString: 'Age Groups'
                                    }
                                  }
                                ]
                              },
                              responsive: true,
                              legend: {position: 'bottom'}
                            }
                          });}
}
if(window.location.href.indexOf("/") != -1)
renderChart();

//if(window.location.href.indexOf("/vllists") != -1) {
$(function () { /*
  $('#vllist thead tr').clone(true).appendTo( '#vllist thead' );
    $('#vllist thead tr:eq(1) th').each( function (i) {
        var title = $(this).text();
        if(title==='Province' || title =='District' || title=='Facility' || title=='Gender' || title=='Age Group')
        $(this).html( '<input type="text" placeholder="'+title+'" size="4"/>' );
        else $(this).html( '' );
        $( 'input', this ).on( 'keyup change', function () {
            if ( table.column(i).search() !== this.value ) {
                table
                    .column(i)
                    .search( '\\b' + this.value + '\\b' ,true, false)
                    .draw();
            }
        } );
    } ); */

  var table = $("#vllist").DataTable({
        order: [[1, 'desc']],
        paging: true,
        pageLength: 10,
        dom: 'Bfrtip',
        buttons: [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5',
            {
              extend: 'pdfHtml5',
              orientation: 'landscape',
              pageSize: 'LEGAL',
              footer: true
          }
        ],
        orderCellsTop: true,
        fixedHeader: true,
        
  });
  });
//}

  $('.knob').knob({
    /*change : function (value) {
     //console.log("change : " + value);
     },
     release : function (value) {
     console.log("release : " + value);
     },
     cancel : function () {
     console.log("cancel : " + this.value);
     },*/
    draw: function () {

      // "tron" case
      if (this.$.data('skin') == 'tron') {

        var a   = this.angle(this.cv)  // Angle
          ,
            sa  = this.startAngle          // Previous start angle
          ,
            sat = this.startAngle         // Start angle
          ,
            ea                            // Previous end angle
          ,
            eat = sat + a                 // End angle
          ,
            r   = true

        this.g.lineWidth = this.lineWidth

        this.o.cursor
        && (sat = eat - 0.3)
        && (eat = eat + 0.3)

        if (this.o.displayPrevious) {
          ea = this.startAngle + this.angle(this.value)
          this.o.cursor
          && (sa = ea - 0.3)
          && (ea = ea + 0.3)
          this.g.beginPath()
          this.g.strokeStyle = this.previousColor
          this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sa, ea, false)
          this.g.stroke()
        }

        this.g.beginPath()
        this.g.strokeStyle = r ? this.o.fgColor : this.fgColor
        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sat, eat, false)
        this.g.stroke()

        this.g.lineWidth = 2
        this.g.beginPath()
        this.g.strokeStyle = this.o.fgColor
        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth + 1 + this.lineWidth * 2 / 3, 0, 2 * Math.PI, false)
        this.g.stroke()

        return false
      }
    }
  });
});




