import { Observable } from 'rxjs';
import {
  PracticeRequestDto, StartResponse, AnswerRequest, AnswerResponse, ResultDto
} from '../practice.models';

/**
 * Port (boundary) for the Practice feature.
 * NOTE: Not wired yet; no behavior change.
 */
export abstract class PracticeApiPort {
  abstract start(dto: PracticeRequestDto): Observable<StartResponse>;
  abstract answer(sessionId: string, req: AnswerRequest): Observable<AnswerResponse>;
  abstract result(sessionId: string): Observable<ResultDto>;
}
