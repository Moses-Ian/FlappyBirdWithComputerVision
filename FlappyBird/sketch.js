let bird;
let pipes;

function setup() {
	let canvas = createCanvas(400, 600);
	canvas.parent('sketch-container');
	bird = new Bird();
	pipes = [];
	pipes.push(new Pipe());
}

function draw() {
  background(0);
	
	if (frameCount % pipePeriod == 0)
		pipes.push(new Pipe());
	
	for (let i=pipes.length-1; i>=0; i--) {
		pipes[i].update();
		pipes[i].show();
		
		if (pipes[i].hit(bird))
			console.log("hit");
		
		if (pipes[i].finished())
			pipes.splice(i, 1);
	}

	bird.update();
	bird.show();
}

function keyPressed() {
	if (key = ' ')
		bird.flap();
	
}