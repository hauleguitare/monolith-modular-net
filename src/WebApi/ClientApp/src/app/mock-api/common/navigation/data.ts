/* eslint-disable */
import { FuseNavigationItem } from '@fuse/components/navigation';
import { UserResponse } from '../../../../@api/user';


const checkPermissions = (item: FuseNavigationItem): boolean => {
    if (!item.meta.protected) {
        return true;
    }

    const accessiblePermissions = item.meta.protected.permissions as string[];

    const user = item.meta['user'] as UserResponse;

    if (!user)
    {
        return true;
    }

    if (!user.roles || !user.roles.length)
    {
        return true;
    }

    const permissions = user.roles.flatMap(e => e.permissions);

    if (!permissions || !permissions.length)
    {
        return true;
    }

    return permissions.some(p => accessiblePermissions.includes(p));
}

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
    },
    {
        id      :   'management',
        title   :   'System Management',
        type    :   'group',
        children    :   [
            {
                id      :   'user-management',
                title  :   'User Management',
                type    :   'basic',
                icon    :   'heroicons_outline:user-group',
                link    :   'management/users',
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
