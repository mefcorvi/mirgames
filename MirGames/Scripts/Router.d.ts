interface IRouter {
    action(controller: string, action: string, params?: any): string;
}

declare var Router: IRouter;