<?php

$m = new MongoClient();
$db = $m->heatmap;

$tagQuery = array();
$sortQuery = array();

$collection = $db->data;

if ($collection->count() == 0) {
	$db->command(array(
	    "create" => 'data'
	));
	$collection = $db->data;
	$collection->ensureIndex(array('gameTimeStamp' => 1));
}

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

	$collection->insert($data);
	return;
}

// GET Request
if (isset($_GET['label'])) {
	$tagQuery['label'] = $_GET['label'];
}

if (isset($_GET['gameTimeStamp'])) {
	$tagQuery['gameTimeStamp'] = array( '$gt' => (float) $_GET['gameTimeStamp'] );
}

if (isset($_GET['sorted'])) {
	$sortQuery['gameTimeStamp'] = 1;
}

if (isset($_GET['ticks'])) {
	$tagQuery['ticks'] = array( '$gt' => $_GET['ticks'] );
}

$cursor = $collection->find($tagQuery)->sort($sortQuery);

echo '[';
foreach ($cursor as $document) {
	$json_string = json_encode($document);
	echo $json_string;
	if ($cursor->hasNext()) {
		 echo ', ';
	}
}
echo ']';


?>