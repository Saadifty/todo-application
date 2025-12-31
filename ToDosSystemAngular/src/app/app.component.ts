import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router, NavigationEnd, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { filter } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-root', 
  standalone: true, 
  imports: [RouterOutlet , CommonModule, MatIconModule, MatButtonModule], 
  templateUrl: './app.component.html', 
  styleUrls: ['./app.component.css'], 
})
export class AppComponent { 
  title = 'ToDosSystemAngular';
  showLogoutButton: boolean = false; 
  showPomodoro: boolean = false; 
  isPomodoroActive: boolean = false; 
  isPomodoroFullscreen: boolean = false; 
  timer: number = 1500; 
  interval: any; 

  @ViewChild('fullscreenContainer') fullscreenContainer!: ElementRef; 

  constructor(private authService: AuthService, private router: Router) {
    this.authService.isLoggedIn$.subscribe((isLoggedIn) => { 
      this.showLogoutButton = isLoggedIn; 
      this.showPomodoro = isLoggedIn; 
    });

    // Listen for route changes
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd)) 
      .subscribe((event: any) => { 
        const isAuthRoute = event.url === '/login' || event.url === '/register'; 
        if (isAuthRoute) { 
          this.showLogoutButton = false; 
          this.showPomodoro = false;
        }
      });
  }

  logout(): void { 
    this.authService.logout(); 
  }

  startPomodoro() { 
    if (this.interval) clearInterval(this.interval); 
    this.timer = 1500; 
    this.isPomodoroFullscreen = true; 
    this.isPomodoroActive = true; 
    this.enterFullscreen(); 
    this.startCountdown(); 
  }

  // Stops Pomodoro: Clears fullscreen and countdown
  stopPomodoro() {
    this.isPomodoroFullscreen = false;
    this.isPomodoroActive = false;
    this.exitFullscreen();
    if (this.interval) clearInterval(this.interval);
  }

  // Starts the countdown timer
  private startCountdown() {
    this.interval = setInterval(() => {
      if (this.timer > 0) {
        this.timer--;
      } else {
        this.stopPomodoro();
        alert('Pomodoro session completed!');
      }
    }, 1000); // Update every second
  }

  // Enters fullscreen mode
  private enterFullscreen() {
    const elem = this.fullscreenContainer?.nativeElement;
    if (elem && elem.requestFullscreen) {
      elem.requestFullscreen();
    }
  }

  // Exits fullscreen mode
  private exitFullscreen() {
    if (document.exitFullscreen) {
      document.exitFullscreen();
    }
  }

  // Dynamically change background color based on time
  get timerColor(): string {
    if (this.timer > 600) return 'green';   // More than 10 minutes
    if (this.timer > 300) return 'yellow';  // Between 5-10 minutes
    return 'red';                           // Less than 5 minutes
  }

  // Formats the timer for display as MM:SS
  get formattedTime(): string {
    const minutes = Math.floor(this.timer / 60).toString().padStart(2, '0');
    const seconds = (this.timer % 60).toString().padStart(2, '0');
    return `${minutes}:${seconds}`;
  }
}