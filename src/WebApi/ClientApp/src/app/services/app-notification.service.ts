import { inject, Injectable } from '@angular/core';
import { FuseConfirmationService } from '../../@fuse/services/confirmation';
import { TranslocoService } from '@ngneat/transloco';

@Injectable({
    providedIn: "root"
})
export class AppNotificationService
{
    private readonly _fuseConfirmationService = inject(FuseConfirmationService);
    private readonly _translocoService = inject(TranslocoService);



    success(title: string = 'title.succeed', message: string = 'message.operationSuccessful')
    {
        return this._fuseConfirmationService.open({
            title: this._translocoService.translate(title),
            message: this._translocoService.translate(message),
            icon: {
                show: true,
                color: 'success',
                name: 'heroicons_outline:check-circle'
            },
            actions: {
                confirm: {
                    show: false,
                },
                cancel: {
                    show: true
                }
            }
        }).afterClosed()
    }

    fail(title: string = 'title.failed', message: string = 'message.operationFailed')
    {
        return this._fuseConfirmationService.open({
            title: this._translocoService.translate(title),
            message: this._translocoService.translate(message),
            icon: {
                show: true,
                color: 'warn',
                name: 'heroicons_outline:exclamation-circle'
            },
            actions: {
                confirm: {
                    show: false,
                },
                cancel: {
                    show: true
                }
            }
        }).afterClosed()
    }
}
