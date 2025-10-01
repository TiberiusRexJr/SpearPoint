import { Component, inject } from '@angular/core';
import { ApiService } from '../../core/api.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'sp-home',
  standalone: true,
  imports: [NgIf],
  templateUrl: './home.component.html'
})
export class HomeComponent {
  api = inject(ApiService);
  health?: string;

  ping() { this.api.getHealth().subscribe(txt => this.health = txt); }
}