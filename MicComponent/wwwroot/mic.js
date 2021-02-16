const constraints = {
	audio: true,
	video: false
};


let recorderButton = document.getElementById('recorder-button'); // the button to start and stop the recording
let recorderButtonDiv = document.getElementById('recorder-button-div'); // the styled div that looks like a record button 
let audioElement; // the audio element that we will feed our recording to
let webAudioRecorder; // our WebAudioRecorder.js recorder yet to be instantiated
let currentlyRecording = false; // a boolean to keep track of whether recording is taking place
let getUserMediaStream; // our stream from getUserMedia
let stream

// public
export async function initialize(micElement, dotnet) {
	try {
		audioElement = micElement;
		stream = await navigator.mediaDevices.getUserMedia(constraints);
		handleSuccess(stream, audioElement, dotnet);
	} catch (e) {
		handleError(e, dotnet);
	}
}


function handleSuccess(stream, audioElement, dotnet) {

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



export async function record(webAudioRecorderJs) {

	// only start the recording stream if there is not another recording in progress
	if (currentlyRecording === false) {

		currentlyRecording = true;

		// if this is a subsequent recording, hide the HTML audio element
		audioElement.controls = false;
		// change the div inside the button so that it looks like a stop button
		recorderButtonDiv.style.backgroundColor = 'rgba(0,0,0,.3)';
		recorderButtonDiv.style.borderRadius = 0;
		// set this to stream so that we can access it outside the scope of the promise 
		// when we need to stop the stream created by getUserMedia 
		getUserMediaStream = stream;
		// the AudioContext that will handle our audio stream
		// if you're in Safari or an older Chrome, you can't use the regular audio context so provide this line to use webkitAudioContext
		let AudioContext = window.AudioContext || window.webkitAudioContext;
		let audioContext = new AudioContext();
		// an audio node that we can feed to a new WebAudioRecorder so we can record/encode the audio data
		let source = audioContext.createMediaStreamSource(getUserMediaStream);
		// the creation of the recorder with its settings:
		webAudioRecorder = new WebAudioRecorder(source, {
			// workerDir: the directory where the WebAudioRecorder.js file lives
			workerDir: webAudioRecorderJs,
			// encoding: type of encoding for our recording ('mp3', 'ogg', or 'wav')
			encoding: 'mp3',
			options: {
				// encodeAfterRecord: our recording won't be usable unless we set this to true
				encodeAfterRecord: true,
				// mp3: bitRate: '160 is default, 320 is max quality'
				mp3: { bitRate: '320' }
			}
		});
		// the method that fires when the recording finishes (triggered by webAudioRecorder.finishRecording() below)
		// the blob is the encoded audio file
		webAudioRecorder.onComplete = (webAudioRecorder, blob) => {
			
			ProcessAudio(blob)
			// create a temporary URL that we can use as the src attribute for our audio element (audioElement)
			let audioElementSource = window.URL.createObjectURL(blob);
			// set this URL as the src attribute of our audio element
			audioElement.src = audioElementSource;
			// add controls so we can see the audio element on the page
			audioElement.controls = true;
			// reset the styles of the button's child div to look like a record button
			recorderButtonDiv.style.backgroundColor = 'red';
			recorderButtonDiv.style.borderRadius = '50%';
		}
		// handles and logs any errors that occur during the encoding/recording process
		webAudioRecorder.onError = (webAudioRecorder, err) => {
			console.error(err);
		}
		// method that initializes the recording
		webAudioRecorder.startRecording();
	}
	else {
		// set this to the array of audio tracks in our getUserMedia stream. In this case we only have one.
		let audioTrack = getUserMediaStream.getAudioTracks()[0];
		// stop that track and end the stream
		// this is not absolutely necessary, but it stops the browser streaming audio inbetween recordings so you should probably do it
		audioTrack.stop();
		// this finishes things up and calls webAudioRecorder.onComplete
		webAudioRecorder.finishRecording();
		currentlyRecording = false;
	}


	function ProcessAudio(blob) {

		fetch(`http://localhost:7071/api/AudioProcess`, {
			method: 'POST',
			body: blob
		});
	}
}
