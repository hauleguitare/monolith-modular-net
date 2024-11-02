/* eslint-disable */
import { FuseNavigationItem } from '@fuse/components/navigation';


export const defaultNavigation: FuseNavigationItem[] = [
    {
        id      :   'user',
        title   :   'User',
        type    :   'group',
        children    :   [
            {
                id      :   'user-settings',
                title   :   'User Settings',
                type    :   'basic',
                icon    :   'heroicons_outline:user-circle',
                link    :   'user/settings'
            }
        ]
    }
];
export const compactNavigation: FuseNavigationItem[] = [
    ...defaultNavigation
];
export const futuristicNavigation: FuseNavigationItem[] = [
    ...defaultNavigation
];
export const horizontalNavigation: FuseNavigationItem[] = [
    ...defaultNavigation
];
