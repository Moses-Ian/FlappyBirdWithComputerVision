let bird;
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
	
	bird = new Bird();
	pipes = [];
	pipes.push(new Pipe());
	
	bgImgX1 = 0;
	bgImgX2 = width;
	
	score = 0;
	
	isPaused = false;
}

function draw() {
	parallax(backgroundImg);
  
	if (frameCount % pipePeriod == 0)
		pipes.push(new Pipe());
	
	for (let i=pipes.length-1; i>=0; i--) {
		pipes[i].update();
		pipes[i].show();
		
		if (pipes[i].hit(bird))
		  gameOver();
		
		if (pipes[i].finished()) {
			pipes.splice(i, 1);
			score++;
		}
	}

	bird.update();
	if (bird.hitFloor() || bird.hitRoof())
		gameOver();
	
	bird.show();
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
	strokeWeight(8);
	rectMode(CENTER);
	fill(255);
	rect(width / 2, height / 2, width - 80, 80);
	fill(0);
	text("Score: " +
		score, width / 2, height / 2);
	noLoop();
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