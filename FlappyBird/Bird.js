class Bird {
	
	constructor() {
		this.x = 64;
		this.y = height/2;
		this.gravity = gravity;
		this.velocity = 0;
	}
	
	update() {
		this.velocity += this.gravity;
		this.y += this.velocity;
		
		if (this.hitFloor()) {
			this.y = height;
			this.velocity = 0;
		}
		
		if (this.hitRoof()) {
			this.y = 0;
			this.velocity = 0;
		}
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
		ellipse(this.x, this.y, 32, 32);
	}
}