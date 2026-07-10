import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error) => {
      const friendlyMessage = error?.error?.error ?? 'An unexpected error occurred. Please try again.';
      console.error(`[HTTP ${error.status}] ${friendlyMessage}`);
      return throwError(() => ({ ...error, friendlyMessage }));
    })
  );
};
