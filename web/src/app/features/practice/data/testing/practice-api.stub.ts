import { of } from 'rxjs';
import { PracticeApiPort } from '../practice-api.port';
import {
  PracticeRequestDto, StartResponse, AnswerRequest, AnswerResponse, ResultDto
} from '../../practice.models';
import { makeStartResponse } from './practice-api.fixture';

export class PracticeApiStub extends PracticeApiPort {
  private total = 3;
  start(_dto: PracticeRequestDto) { return of<StartResponse>(makeStartResponse(this.total)); }
  answer(_sid: string, _req: AnswerRequest) { return of<AnswerResponse>({ isCorrect: true, done: false }); }
  result(_sid: string) { return of<ResultDto>({ correct: this.total, total: this.total, percent: 100 }); }
}
