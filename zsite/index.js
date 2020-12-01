

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
    },

    
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralloadlists', function(error, response, body) {
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
    res.render('index', {title: 'LIS Dashboard',loc:'index',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body),obj3 : JSON.parse(results[2].body)});
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
  },
  
function(callback) {
  request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralloadlists'+qry, function(error, response, body) {
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
  
  res.render('index', {title: 'LIS Dashboard',loc:'index',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body), obj3 : JSON.parse(results[2].body), province: province, district: district, facility: facility, start: stdate, end : edate});
}) 
 
    
});  

// VL Tabular calls
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
    res.render('vllists', {title: 'LIS Dashboard',loc:'index',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body)});
  })
}); 
   
 
app.post('/vllists',function(req,res){
  var dates = req.body.daterange.split(" - ");
  var stdate=dates[0];
  var edate=dates[1];
  var province = req.body.province;
  var district = req.body.district;
  //var facility =req.body.facility;
  var qry2="?Province="+province+"&District="+district+"&StDate="+stdate+"&EDate="+edate;
  console.log(qry2);
  
async.parallel([
function(callback) {
  request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/viralloadlists'+qry2, function(error, response, body) {
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

res.render('vllists', {title: 'LIS Dashboard',loc:'index',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body), province: province, district: district, start: stdate, end : edate});
}) 

  
});  

// Trend calls


app.get("/vltrends", (req, res, next) => {
  async.parallel([
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/vltrends', function(error, response, body) {
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
    res.render('vltrends', {title: 'LIS Dashboard',loc:'vltrends',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body)});
  })
}); 

  app.post('/vltrends',function(req,res){
    
    var province = req.body.province;
    var district = req.body.district;
    var facility =req.body.facility;
    var qry="?Province="+province+"&District="+district+"&Facility="+facility;
    //console.log(qry);
    
 async.parallel([
  function(callback) {
    request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/vltrends'+qry, function(error, response, body) {
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
  
  res.render('vltrends', {title: 'LIS Dashboard',loc:'vltrends',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body), province: province, district: district, facility: facility});
}) 
 
    
});  


//EID Trends

app.get("/eidtrends", (req, res, next) => {
  async.parallel([
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/eidtrends', function(error, response, body) {
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
    res.render('eidtrends', {title: 'LIS Dashboard',loc:'eidtrends',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body)});
  })
}); 

  app.post('/eidtrends',function(req,res){
    
    var province = req.body.province;
    var district = req.body.district;
    var facility =req.body.facility;
    var qry="?Province="+province+"&District="+district+"&Facility="+facility;
    //console.log(qry);
    
 async.parallel([
  function(callback) {
    request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/eidtrends'+qry, function(error, response, body) {
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
  
  res.render('eidtrends', {title: 'LIS Dashboard',loc:'eidtrends',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body), province: province, district: district, facility: facility});
}) 
 
    
});  




// EID calls
app.get("/eidtests", (req, res, next) => {
  async.parallel([
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/eid', function(error, response, body) {
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
    res.render('eidtests', {title: 'LIS Dashboard - EID Tests',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body)});
  })
}); 


app.post('/eidtests',function(req,res){
  var dates = req.body.daterange.split(" - ");
  var stdate=dates[0];
  var edate=dates[1];
  var province = req.body.province;
  var district = req.body.district;
  var facility =req.body.facility;
  var qry2="?Province="+province+"&District="+district+"&Facility="+facility+"&StDate="+stdate+"&EDate="+edate;
  console.log(qry2);
  
async.parallel([
function(callback) {
  request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/eid'+qry2, function(error, response, body) {
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

res.render('eidtests', {title: 'LIS Dashboard - EID Tests',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body), province: province, district: district, start: stdate, end : edate});
}) 

  
});  


// VL Client calls
app.get("/vlclients", (req, res, next) => {
  async.parallel([
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/vlclientinfo', function(error, response, body) {
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
    },
    function(callback) {
      request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/vlcblists', function(error, response, body) {
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
    res.render('vlclients', {title: 'LIS Dashboard',loc:'index',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body),obj3 : JSON.parse(results[2].body)});
  })
}); 

  app.post('/vlclients',function(req,res){
    var dates = req.body.daterange.split(" - ");
    var stdate=dates[0];
    var edate=dates[1];
    var province = req.body.province;
    var district = req.body.district;
    var facility =req.body.facility;
    var qry="?Province="+province+"&District="+district+"&Facility="+facility+"&StDate="+stdate+"&EDate="+edate;
    console.log(qry);
    
 async.series([
  function(callback) {
    request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/vlclientinfo'+qry, function(error, response, body) {
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
  },
  function(callback) {
    request('http://lis.moh.gov.zm/api/api/openldr/general/'+apikey+'/'+apiversion+'/json/vlcblists'+qry, function(error, response, body) {
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
  
  res.render('vlclients', {title: 'LIS Dashboard',loc:'index',obj: JSON.parse(results[0].body),obj2 : JSON.parse(results[1].body),obj3 : JSON.parse(results[2].body), province: province, district: district, facility: facility, start: stdate, end : edate});
}) 
 
    
});  


  

/**
 * Server Activation
 */
   
var server = app.listen(port, () => {
    console.log('Your app is runnin at http://localhost:%s', port);
});