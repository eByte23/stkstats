import { Team, Teams } from "@/types"
import { GetStaticProps } from "next"
import fs from "fs"

type TeamPageProps = {
    teams: Team[]
}


const TeamsPage = (props: TeamPageProps) => {

    return (
        <div className="flex flex-col justify-center items-center mx-auto ">
            <h1 className="text-3xl">Teams</h1>
            <div>
                {props.teams.map((team) => (
                    <div key={team.ShortId}>
                        <a href={`/teams/${team.ShortId}`}>{team.TeamName}</a>
                        </div>
                ))}
            </div>
        </div>
    )
}

export default TeamsPage


export const getStaticProps: GetStaticProps<TeamPageProps> = async () => {


    const content = fs.readFileSync(process.cwd() + `/data/teams.json`).toString()

    const teams = JSON.parse(content) as Team[] || []

    return {
        props: {
            teams: teams || {}
        },
    }
}