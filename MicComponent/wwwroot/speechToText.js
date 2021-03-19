let jsTextElement;

export async function initialize(textElement, dotnet) {
	try {
		jsTextElement = textElement;

		
		//handleSuccess(stream, audioElement, dotnet);
	} catch (e) {
		//handleError(e, dotnet);
	}
}

export async function getText() {
	debugger;
	let id = window.localStorage.getItem('id');

	//fetch(`http://localhost:7071/api/Download/?id=${id}`, {
	//	method: 'GET',
	//})
	//	.then(response => response.json())	
	//	.then(data => {
	//		console.log(data);			
	//	})
	//	.catch((error) => { console.log(error) });

}