import { Component, inject } from '@angular/core';
import { ApiService } from '../../core/api.service';
import { NgFor } from '@angular/common';

@Component({
  selector: 'sp-practice',
  standalone: true,
  imports: [NgFor],
  templateUrl: './practice.component.html'
})
export class PracticeComponent {
  private api = inject(ApiService);
  questions: any[] = [];
  ngOnInit() { this.api.getQuestions().subscribe(q => this.questions = q); }
}
