class Bird {
	
	constructor() {
		this.x = 64;
		this.y = height/2;
		this.gravity = gravity;
		this.velocity = 0;
		this.r = radius;
	}
	
	update() {
		this.velocity += this.gravity;
		this.y += this.velocity;
	}
	
	hitFloor() {
		return this.y > height;
	}
	
	hitRoof() {
		return this.y < 0;
	}
	
	flap() {
		this.velocity = -flapStrength; 
	}
	
  show() {
    fill(255);
    push();
    imageMode(CENTER);
    translate(this.x, this.y);
    if (this.velocity < 0) {
      rotate(-35);
    } else {
      rotate(35);
    }
    image(birdImg, 0, 0, this.r * 2, this.r * 2);
    //ellipse(0, 0, hurtBoxRadius * 2);
    pop();
  }
}