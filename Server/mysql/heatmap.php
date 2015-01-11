<?php
error_reporting(E_ALL);
// Put your info here
$DB_HOST	= "127.0.0.1";
$DB_USERNAME 	= "username";
$DB_PASSWORD 	= "password";
$DB_NAME 	= "Heatmap";
$DB_TABLENAME 	= "Data";

// Create connection
$mysqli = new mysqli($DB_HOST, $DB_USERNAME, $DB_PASSWORD);

// Check connection
if (mysqli_connect_errno()) {
  echo "Failed to connect to MySQL: " . mysqli_connect_error();
  exit();
}

// Create database
$sql = "CREATE DATABASE IF NOT EXISTS " . $DB_NAME;
$mysqli->query($sql);


$mysqli = new mysqli($DB_HOST, $DB_USERNAME, $DB_PASSWORD, $DB_NAME);
$sql = "
	CREATE TABLE IF NOT EXISTS " . $DB_TABLENAME . " (
	_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	x FLOAT(10, 6) NOT NULL,
	y FLOAT(10, 6) NOT NULL,
	z FLOAT(10, 6) NOT NULL,
	label VARCHAR(100) NOT NULL,
	ticks VARCHAR(20) NOT NULL,
	gameTimeStamp FLOAT(10, 6) NOT NULL
);";

$mysqli->query($sql);

// POST Request
if (isset($_POST["x"]) && isset($_POST["y"]) && isset($_POST["z"]) 
	&& isset($_POST["label"]) 
	&& isset($_POST["ticks"]) 
	&& isset($_POST["gameTimeStamp"])) {
	$data = array(
	    "x"  => (float) $_POST["x"],
	    "y" => (float) $_POST["y"],
	    "z" => (float) $_POST["z"],
	    "label" => $_POST["label"],
	    "ticks" => $_POST["ticks"],
	    "gameTimeStamp" => (float) $_POST["gameTimeStamp"]
	);

	$prp_stmt = "INSERT INTO " . $DB_TABLENAME . " ( x, y, z, label, ticks, gameTimeStamp ) VALUES (?, ?, ?, ?, ?, ?)";
	if ($stmt = mysqli_prepare($mysqli, $prp_stmt)) {

 		$stmt->bind_param("dddssd", $data['x'], $data['y'], $data['z'], $data['label'], $data['ticks'], $data['gameTimeStamp']); 
	    $stmt->execute();
	    $stmt->close();
	}

	return;
}


$sql = "SELECT x, y, z, label, ticks, gameTimeStamp FROM " . $DB_TABLENAME;

$connectorWord = "WHERE";
// GET Request
if (isset($_GET['label'])) {
	$sql = $sql . ' ' . $connectorWord . ' label = ' . '"' . $_GET['label'] . '"';
	$connectorWord = "AND";
}

if (isset($_GET['gameTimeStamp'])) {
	$sql = $sql . ' ' . $connectorWord . ' gameTimeStamp > ' . (float) $_GET['gameTimeStamp'];
	$connectorWord = "AND";
}

if (isset($_GET['sorted'])) {
	$sql = $sql . ' ' . 'ORDER BY gameTimeStamp';
}

if (isset($_GET['ticks'])) {
	$sql = $sql . ' ' . $connectorWord . ' ticks > ' . $_GET['ticks'];
	$connectorWord = "AND";
}

$result = $mysqli->query($sql);

echo '[';
$commaMarker = false;
while ( $row = $result->fetch_row() ) {
	if ($commaMarker) {
		echo ', ';
	}
	$commaMarker = true;

    $data["x"] = $row[0];
    $data["y"] = $row[1];
    $data["z"] = $row[2];
    $data["label"] = $row[3];
    $data["ticks"] = $row[4];
    $data["gameTimeStamp"] = $row[5];

    $json_string = json_encode($data);
	echo $json_string;
}

echo ']';
$result->close();

?>
