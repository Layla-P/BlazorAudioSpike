const constraints = {
	audio: true,
	video: false
};

// public
export async function initialize(micElement, dotnet) {
	try {
		loadAudioScript('./_content/MicComponent/WebAudioRecorder.js');
		let stream = await navigator.mediaDevices.getUserMedia(constraints);
		handleSuccess(stream, micElement, dotnet);
	} catch (e) {
		handleError(e, dotnet);
	}
}

export function getMicRecording(micElement) {
	let mic = micElement;
	
}

function handleSuccess(stream, micElement, dotnet) {
	
	console.log("We got here!!");
}

function handleError(error, dotnet) {
	errorMsg(`getUserMedia error: ${error.name}`, error, dotnet);
}

function errorMsg(msg, error, dotnet) {
	if (typeof error !== 'undefined') {
		console.log(error);
	}
	//dotnet.invokeMethodAsync("MicError", msg);
}

function loadAudioScript(scriptPath) {
	var script = document.createElement("script");
	script.src = scriptPath;
	script.type = "text/javascript";
	console.log(scriptPath + " created");

	// flag as loading/loaded
	//loaded[scriptPath] = true;

	// if the script returns okay, return resolve
	script.onload = function () {
		console.log(scriptPath + " loaded ok");
		//resolve(scriptPath);
	};

	// if it fails, return reject
	script.onerror = function () {
		console.log(scriptPath + " load failed");
		//reject(scriptPath);
	}

	// scripts will load at end of body
	document["body"].appendChild(script);
}