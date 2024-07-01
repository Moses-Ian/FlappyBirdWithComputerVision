const POP_SIZE = 250;
let birds;
let savedBirds;
let pipes;
let bgImgX1;
let bgImgX2;
let backgroundImg;
let score;
let isPaused;

function preload() {
  birdImg = loadImage('assets/bird.png');
  pipeImg = loadImage('assets/pipes.png');
  pipeRevImg = loadImage('assets/pipes_reverse.png');
  backgroundImg = loadImage('assets/background.png');
}

function setup() {
	let canvas = createCanvas(400, 600);
	canvas.parent('sketch-container');
	
	angleMode(DEGREES);
	rectMode(CORNER);
	imageMode(CORNER);
	textAlign(CENTER, CENTER);
  textStyle(BOLD);
  textSize(50);
	
	birds = [];
	for (i=0; i<POP_SIZE; i++)
		birds.push(new Bird());
	
	
	pipes = [];
	pipes.push(new Pipe());
	
	bgImgX1 = 0;
	bgImgX2 = width;
	
	score = 0;
	
	isPaused = false;
	savedBirds = [];
}

function draw() {
	parallax(backgroundImg);
  
	if (frameCount % pipePeriod == 0)
		pipes.push(new Pipe());
	
	for (let i=pipes.length-1; i>=0; i--) {
		pipes[i].update();
		pipes[i].show();
		
		for (let j=birds.length-1; j>=0; j--) {
			if (pipes[i].hit(birds[j]))
				savedBirds.push(...birds.splice(j,1));
		}
		
		if (pipes[i].finished()) {
			pipes.splice(i, 1);
			score++;
		}
	}

	for (let j=birds.length-1; j>=0; j--) {
		birds[j].think(pipes);
		birds[j].update();
		if (birds[j].hitFloor() || birds[j].hitRoof())
			savedBirds.push(...birds.splice(j,1));
		else
			birds[j].show();
	}
	
	if (birds.length == 0) {
		nextGeneration();
		pipes = [];
		frameCount = 0;
	}
}

function keyPressed() {
	if (key == ' ')
		bird.flap();
	else if (key == 'p' && isPaused)
		resume();
	else if (key == 'p' && !isPaused)
		pause();
	else if (key == 'r')
		restart();
}

function parallax(img) {
	background(0);
	image(img, bgImgX1, 0, width, height);
	image(img, bgImgX2, 0, width, height);
	bgImgX1 -= bgScrollSpeed;
	bgImgX2 -= bgScrollSpeed;
	
	if (bgImgX1 < -width)
		bgImgX1 = bgImgX2 + width;
	if (bgImgX2 < -width)
		bgImgX2 = bgImgX1 + width;
}

function gameOver() {
	
}

function pause() {
	noLoop();
	isPaused = true;
}

function resume() {
	loop();
	isPaused = false;
}

function restart() {
	bird = new Bird();
	pipes = [];
	pipes.push(new Pipe());
	score = 0;
	isPaused = false;
	frameCount = 0;
	loop();
}