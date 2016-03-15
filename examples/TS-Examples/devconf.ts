//nSwaggerVersion:0.0.4
// This file was automatically generated by nSwagger. Changes made to this file will be lost if nSwagger is run again. See https://github.com/rmaclean/nswagger for more information.
// This file was last generated at: 2016-03-15T14:15:08.5978768Z
namespace nSwagger {
    export module DevConfRatings {
        export interface GetRatings {
            Email: string;
        }

        export interface RatingSession {
            Comment: string;
            Order: number;
            Rating: number;
            SessionId: number;
        }

        export interface Rating {
            Email: string;
            Session1: RatingSession;
            Session2: RatingSession;
            Session3: RatingSession;
            Session4: RatingSession;
            Session5: RatingSession;
            Session6: RatingSession;
            Session7: RatingSession;
            Session8: RatingSession;
        }

        export interface TimeSlot {
            End: string;
            Order: number;
            Sessions: Array<Session>;
            Start: string;
        }

        export interface Session {
            Id: number;
            Presenter: string;
            Title: string;
            Track: string;
        }

        export interface Rating_PostGetRatingRequest {
            request: GetRatings;
        }

        export interface Rating_PostAddRatingRequest {
            rating: Rating;
        }

        export interface API {
            setToken(value: string, headerOrQueryName: string, isQuery: boolean): void;
            Rating_PostGetRating(parameters: Rating_PostGetRatingRequest): PromiseLike<Array<RatingSession>>;
            Rating_PostAddRating(parameters: Rating_PostAddRatingRequest): PromiseLike<string>;
            Session_GetSessions(): PromiseLike<Array<TimeSlot>>;
        }
    }
}
