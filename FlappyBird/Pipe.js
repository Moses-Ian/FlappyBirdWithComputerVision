class Pipe {
	constructor() {
		this.top = random(height/2);
		this.bottom = random(height/2);
		this.x = width;
		this.w = pipeWidth;
		this.speed = pipeSpeed;
	}
	
	update() {
		this.x -= this.speed;
	}
	
	finished() {
		return this.x + this.w < 0;
	}
	
	hit(bird) {
		if (bird.x < this.x)
			return false;
		if (bird.x > this.x + this.w)
			return false;
		if (bird.y > this.top && bird.y < height - this.bottom)
			return false;
		return true;
  }
	
	show() {
		fill(255);
		
		// top pipe
		rect(this.x, 0, this.w, this.top);
		
		// bottom pipe
		rect(this.x, height-this.bottom, this.w, this.bottom);
	}
}