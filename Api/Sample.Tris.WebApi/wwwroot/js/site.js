function apiRequest(url, cb) {
  $.getJSON(url)
   .done(function(json){
       cb(null, json);
   })
   .fail(function(jqxhr, textStatus, error) {
       cb(jqxhr.responseText, null);
   });
}

function apiGetTriangleByPoints(p1, p2, p3, cb) {
  var params = {
      p1: p1.x + ',' + p1.y,
      p2: p2.x + ',' + p2.y,
      p3: p3.x + ',' + p3.y
  };

  var query = Object.keys(params)
      .map(k => k + '=' + params[k])
      .join('&');

  apiRequest('api/triangles/query?' + query, cb);
}

function apiGetTriangleByLabel(label, cb) {
  apiRequest('api/triangles/' + label, cb);
}

function parsePoint(pointStr) {
  var match = pointStr.match(/^([0-9]+),([0-9]+)/);

  if (!match) {
      return null;
  }

  return {
      x: match[1],
      y: match[2]
  };
}

function parseLabel(labelStr) {
  var match = labelStr.match(/^([A-Z]{1,3})([1-9][0-9]*)$/);

  if (!match) {
      return null;
  }

  return match[0];
}

function formatPoint(pointXY) {
  return '(' + pointXY.x + ',' + pointXY.y + ')';
}

function formatTrianglePoints(triangle) {
  return formatPoint(triangle.p1) + ' ' + formatPoint(triangle.p2) + ' ' + formatPoint(triangle.p3);
}

$(document).ready(function(){
  var $btnFindByCoords = $('#btn-findbycoords');
  var $btnFindByLabel = $('#btn-findbylabel');
  var $divOutputByCoords = $('#output-bycoords');
  var $divOutputByLabel = $('#output-bylabel');
  var inputPoints = {
      $p1: $('#p1-input'),
      $p2: $('#p2-input'),
      $p3: $('#p3-input')
  };
  var $inputLabel = $('#label-input');

  function onFindByCoordsClicked() {
    var p1 = parsePoint(inputPoints.$p1.val());
    var p2 = parsePoint(inputPoints.$p2.val());
    var p3 = parsePoint(inputPoints.$p3.val());

    if (!p1 || !p2 || !p3) {
      var invalidPoints = [];

      if (!p1) invalidPoints.push('p1');
      if (!p2) invalidPoints.push('p2');
      if (!p3) invalidPoints.push('p3');

      return $divOutputByCoords.text('Invalid coords entered for ' + invalidPoints.join(', '));
    }

    apiGetTriangleByPoints(p1,p2,p3, function(err, data) {
      if (err) {
        return $divOutputByCoords.text('Result: ' + err);
      }
      return $divOutputByCoords.text('Result: ' + data.gridLabel);
    });
  }

  function onFindByLabelClicked() {
    var label = parseLabel($inputLabel.val());

    if (!label) {
      return $divOutputByLabel.text('Invalid label format entered');
    }

    apiGetTriangleByLabel(label, function(err, data) {
      if (err) {
        return $divOutputByLabel.text('Result: ' + err);
      }
      return $divOutputByLabel.text('Result: ' + formatTrianglePoints(data));
    });
  }

  $btnFindByCoords.click(onFindByCoordsClicked);
  $btnFindByLabel.click(onFindByLabelClicked);
});