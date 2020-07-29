
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

    

});

$(document).ready(function () {
  $('province').change(function (e) {
      var value = $('province').val();
      dlist = Array.from(new Set(obj2.filter(function(e){return e.Province==value}).map(item=>item.District)));
      console.log(value);
  });


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


