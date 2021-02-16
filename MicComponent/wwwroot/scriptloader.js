export async function loadScript(scriptPath) {
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

export async function mp3Lame() {
	debugger;
	var script = document.createElement("script");

	script.innerHTML = `Mp3LameEncoderConfig = {memoryInitializerPrefixURL: "./_content/MicComponent/webAudioRecorderJs/"}`;
	document["body"].appendChild(script);

	//loadScript("./_content/MicComponent/webAudioRecorderJs/Mp3LameEncoder.min.js");
}