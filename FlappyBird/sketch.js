let bird;
let pipes;
let bgImgX1;
let bgImgX2;
let backgroundImg;

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
	
	bird = new Bird();
	pipes = [];
	pipes.push(new Pipe());
	
	bgImgX1 = 0;
	bgImgX2 = width;
}

function draw() {
	parallax(backgroundImg);
  //background(backgroundImg);
	
	if (frameCount % pipePeriod == 0)
		pipes.push(new Pipe());
	
	for (let i=pipes.length-1; i>=0; i--) {
		pipes[i].update();
		pipes[i].show();
		//pipes[i].hit(bird);
		
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