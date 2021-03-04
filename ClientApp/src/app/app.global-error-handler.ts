import * as Sentry from "@sentry/angular";
import { ToastrService } from "ngx-toastr";
import {
  ErrorHandler,
  Injectable,
  Inject,
  Injector,
  isDevMode,
} from "@angular/core";

/**
 * Handle any errors thrown by Angular application
 */
@Injectable()
export class GlobalErrorHandler extends ErrorHandler {
  constructor(@Inject(Injector) private readonly injector: Injector) {
    super();
  }

  handleError(error) {
    if (!isDevMode()) {
      Sentry.captureException(error.originalError || error);
    } else {
      throw error;
    }

    console.log("Handling error: " + error);

    this.toastrService.error("An unexpected error happened.", "Error", {
      onActivateTick: true,
      closeButton: true,
      timeOut: 5000,
    });

    super.handleError(error);
  }

  /**
   * Need to get ToastrService from injector rather than constructor injection to avoid cyclic dependency error
   * @returns {}
   */
  private get toastrService(): ToastrService {
    return this.injector.get(ToastrService);
  }
}
