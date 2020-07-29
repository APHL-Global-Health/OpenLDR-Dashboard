

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
   request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralload', function(error,response,body) {
      if (!error && response.statusCode == 200) {
        var o = JSON.parse(body);
        res.render('index', {title: 'LIS Dashboard',obj: o});
      }
  });/*
  var o;
  var levels;
  async.parallel([
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralload', function(error, response, body) {
        if (!error && response.statusCode == 200) {
          o = JSON.parse(body);
          return callback(null, response);
        }
        return callback(error || new Error('Response non-200'));
      })
    },
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/orglevel', function(error, response, body) {
        if (!error && response.statusCode == 200) {
          levels = JSON.parse(body);
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
    
    res.render('index', {title: 'LIS Dashboard',obj: o,obj2 : levels});
  })*/
}); 

  app.post('/',function(req,res){
    var dates = req.body.daterange.split(" - ");
    var stdate=dates[0];
    var edate=dates[1];
    var qry="?Province="+req.body.province+"&District="+req.body.district+"&StDate="+stdate+"&EDate="+edate;
    console.log(qry);
    request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralload'+qry, function(error,response,body) {
      if (!error && response.statusCode == 200) {
        var o = JSON.parse(body);
        res.render('index', {title: 'LIS Dashboard',obj: o, start: stdate, end : edate});
      }
 });  
 
    
});  
   
 

  

/**
 * Server Activation
 */
   
var server = app.listen(port, () => {
    console.log('Your app is runnin at http://localhost:%s', port);
});