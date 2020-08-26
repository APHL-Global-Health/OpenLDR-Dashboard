

// index.js

/**
 * Required External Modules
 */

const express = require('express');
const bodyParser = require('body-parser');
const path = require('path');
var async = require('async');
const apikey='e98389ca62d99875ba7a4e0f2929960b';
const apiversion='v1';
/**
 * App Variables
 */

const app = express();
const port = process.env.PORT || 5000;
const request = require('request');
var obj2;
/**
 *  App Configuration
 */

app.set("view engine", "pug");
app.set("views", path.join(__dirname, "views"));
app.use(express.static(path.join(__dirname ,"public")));
app.use(bodyParser.urlencoded({extended: true}));


/**
 * Routes Definitions
 */

app.get("/", (req, res, next) => {
  async.parallel([
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralload', function(error, response, body) {
        if (!error && response.statusCode == 200) {
          return callback(null, response);
        }
        return callback(error || new Error('Response non-200'));
      })
    },
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/orglevel', function(error, response, body) {
        if (!error && response.statusCode == 200) {
          return callback(null, response);
        }
        return callback(error || new Error('Response non-200'));
      })
    }
  ],
  // optional callback
  function(err, results) {
    if (err) {
      // Handle or return error
    }
    res.render('index', {title: 'LIS Dashboard',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body)});
  })
}); 

  app.post('/',function(req,res){
    var dates = req.body.daterange.split(" - ");
    var stdate=dates[0];
    var edate=dates[1];
    var province = req.body.province;
    var district = req.body.district;
    var facility =req.body.facility;
    var qry="?Province="+province+"&District="+district+"&Facility="+facility+"&StDate="+stdate+"&EDate="+edate;
    //console.log(qry);
    
 async.parallel([
  function(callback) {
    request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralload'+qry, function(error, response, body) {
      if (!error && response.statusCode == 200) {
        return callback(null, response);
      }
      return callback(error || new Error('Response non-200'));
    })
  },
  function(callback) {
    request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/orglevel', function(error, response, body) {
      if (!error && response.statusCode == 200) {
        return callback(null, response);
      }
      return callback(error || new Error('Response non-200'));
    })
  }
],
// optional callback
function(err, results) {
  if (err) {
    // Handle or return error
  }
  
  res.render('index', {title: 'LIS Dashboard',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body), province: province, district: district, facility: facility, start: stdate, end : edate});
}) 
 
    
});  


app.get("/vllists", (req, res, next) => {
  async.parallel([
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralloadlists', function(error, response, body) {
        if (!error && response.statusCode == 200) {
          return callback(null, response);
        }
        return callback(error || new Error('Response non-200'));
      })
    },
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/orglevel', function(error, response, body) {
        if (!error && response.statusCode == 200) {
          return callback(null, response);
        }
        return callback(error || new Error('Response non-200'));
      })
    }
  ],
  // optional callback
  function(err, results) {
    if (err) {
      // Handle or return error
    }
    res.render('vllists', {title: 'LIS Dashboard',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body)});
  })
}); 
   
 
app.post('/vllists',function(req,res){
  var dates = req.body.daterange.split(" - ");
  var stdate=dates[0];
  var edate=dates[1];
  var province = req.body.province;
  var district = req.body.district;
  //var facility =req.body.facility;
  var qry="?Province="+province+"&District="+district+"&StDate="+stdate+"&EDate="+edate;
  //console.log(qry);
  
async.parallel([
function(callback) {
  request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralloadlists'+qry, function(error, response, body) {
    if (!error && response.statusCode == 200) {
      return callback(null, response);
    }
    return callback(error || new Error('Response non-200'));
  })
},
function(callback) {
  request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/orglevel', function(error, response, body) {
    if (!error && response.statusCode == 200) {
      return callback(null, response);
    }
    return callback(error || new Error('Response non-200'));
  })
}
],
// optional callback
function(err, results) {
if (err) {
  // Handle or return error
}

res.render('vllists', {title: 'LIS Dashboard',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body), province: province, district: district, start: stdate, end : edate});
}) 

  
});  

  

/**
 * Server Activation
 */
   
var server = app.listen(port, () => {
    console.log('Your app is runnin at http://localhost:%s', port);
});